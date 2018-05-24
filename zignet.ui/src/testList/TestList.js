import React, { Component } from 'react';
import ZigNetApi from '../api/ZigNetApi'
import LatestTestResultsList from './LatestTestResultsList'

class TestList extends Component {
  constructor(props) {
    super(props);

    this.zigNetApi = new ZigNetApi(process.env.REACT_APP_API_BASE_URL + 'api/');
    this.state = { 
      suiteName: '',
      latestTestResults: []
    }
  }

  componentDidMount() {
    this._getLatestTestResultsForSuite();
    this.intervalId = setInterval(() => this._getLatestTestResultsForSuite(), 20000);
  }

  componentWillUnmount() {
    clearInterval(this.intervalId);
  }

  _getLatestTestResultsForSuite() {
    this.zigNetApi.getLatestTestResultsForSuite(this.props.suiteId, this.props.grouped)
      .then(response => {
        this.setState({
          suiteName: response.SuiteName,
          latestTestResults: response.LatestTestResults
        })
      })
      .catch(error => alert(error));
  }

  render() {
    return (
      <div>
        <LatestTestResultsList 
          suiteName={this.state.suiteName}
          suiteId={this.props.suiteId}
          testResults={this.state.latestTestResults}
          testResultsGrouped={this.props.grouped} />
      </div>
    );
  }
}

export default TestList;
