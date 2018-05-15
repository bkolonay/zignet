import React, { Component } from 'react';
import TestResultRow from './TestResultRow'
import { Link } from 'react-router-dom'

class LatestTestResultsList extends Component {

  _getGroupButton() {
    var suiteId = this.props.suiteId;
    if (this.props.testResultsGrouped)
      return <a href={"/" + suiteId} className="btn btn-outline-primary float-right" role="button" title="Click to ungroup results">Ungroup</a>
    else
      return <a href={"/" + suiteId + "?group=true"} className="btn btn-primary float-right" role="button" title="Click to group tests by environment">Group</a>
  }  

  render() {
    return (
      <div className="container">
        <h4><Link to='/'>ZigNet</Link></h4>
        <h2 className="text-center">
          <span>{this.props.suiteName}</span>
          {this._getGroupButton()}
        </h2>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID}
                                testResult={testResult} />
            )}        
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
