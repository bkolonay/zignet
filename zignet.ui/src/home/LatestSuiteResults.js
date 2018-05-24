import React, { Component } from 'react';
import ZigNetApi from '../api/ZigNetApi'
import SuiteResults from './SuiteResults'

class LatestSuiteResults extends Component {
  constructor(props) {
    super(props);

    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    this.state = { latestSuiteResults: [] }
  }

  componentDidMount() {
    this._getLatestSuiteResults();
    this.intervalId = setInterval(() => this._getLatestSuiteResults(), 10000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestSuiteResults() {
    this.zigNetApi.getLatestSuiteResults(this.props.grouped, this.props.debug)
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
        <SuiteResults suiteResultsGrouped={this.props.grouped} suiteResults={this.state.latestSuiteResults} />
      </div>
    );
  }
}

export default LatestSuiteResults;
