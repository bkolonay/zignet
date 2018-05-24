import React, { Component } from 'react';
import ZigNetApi from '../api/ZigNetApi'
import SuiteResults from './SuiteResults'

class LatestSuiteResults extends Component {
  constructor(props) {
    super(props);

    if (this.props.location.search.indexOf('group=true') !== -1)
      this.suiteResultsGrouped = true;
    else
      this.suiteResultsGrouped = false;
    if (this.props.location.search.indexOf('debug=true') !== -1)
      this.showDebugSuites = true;
    else
      this.showDebugSuites = false;    

    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    this._getLatestSuiteResults(this.suiteResultsGrouped, this.showDebugSuites);
    this.intervalId = setInterval(() => this._getLatestSuiteResults(this.suiteResultsGrouped, this.showDebugSuites), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestSuiteResults(suiteResultsGrouped, showDebugSuites) {
    this.zigNetApi.getLatestSuiteResults(suiteResultsGrouped, showDebugSuites)
      .then(response => {
        this.setState({
          latestSuiteResults: response
        })
      })
      .catch(error => alert(error));
  }

  render() {
    return (
      <div>
        <h1 className="text-center">ZigNet</h1>
        <SuiteResults suiteResultsGrouped={this.suiteResultsGrouped} suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default LatestSuiteResults;
